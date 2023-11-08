import { http } from './http';
import { getAccessToken } from './Auth';

export interface QuestionData {
  questionId: number;
  title: string;
  content: string;
  userName: string;
  created: Date;
  answers: AnswerData[];
}

export interface QuestionDataFromServer {
  questionId: number;
  title: string;
  content: string;
  userName: string;
  created: string;
  answers: Array<{
    answerId: number;
    content: string;
    userName: string;
    created: string;
  }>;
}

export interface AnswerData {
  answerId: number;
  content: string;
  userName: string;
  created: Date;
}

export interface PostQuestionData {
  title: string;
  content: string;
  userName: string;
  created: Date;
}

export const getQuestion = async (
  questionId: number,
): Promise<QuestionData | null> => {
  const result = await http<QuestionDataFromServer>({
    path: `questions/${questionId}`,
  });
  if (result.ok && result.body) {
    return mapQuestionFromServer(result.body);
  } else {
    return null;
  }
};

const questions: QuestionData[] = [
  {
    questionId: 1,
    title: 'Why should I learn TypeScript?',
    content:
      'TypeScript seems to be getting popular so I wondered whether it is worth my time learning it? What benefits does it give over JavaScript?',
    userName: 'Bob',
    created: new Date(),
    answers: [
      {
        answerId: 1,
        content: 'To catch problems earlier speeding up your developments',
        userName: 'Jane',
        created: new Date(),
      },
      {
        answerId: 2,
        content:
          'So, that you can use the JavaScript features of tomorrow, today',
        userName: 'Fred',
        created: new Date(),
      },
    ],
  },
  {
    questionId: 2,
    title: 'Which state management tool should I use?',
    content:
      'There seem to be a fair few state management tools around for React - React, Unstated, ... Which one should I use?',
    userName: 'Bob',
    created: new Date(),
    answers: [],
  },
];

export const mapQuestionFromServer = (
  question: QuestionDataFromServer,
): QuestionData => ({
  ...question,
  created: new Date(question.created),
  answers: question.answers
    ? question.answers.map((answer) => ({
        ...answer,
        created: new Date(answer.created),
      }))
    : [],
});

export const getUnansweredQuestions = async (): Promise<QuestionData[]> => {
  const result = await http<QuestionDataFromServer[]>({
    path: 'questions/unanswered',
  });
  if (result.ok && result.body) {
    return result.body.map(mapQuestionFromServer);
  } else {
    return [];
  }
};

const wait = async (ms: number): Promise<void> => {
  return new Promise((resolve) => setTimeout(resolve, ms));
};

export const searchQuestions = async (
  criteria: string,
): Promise<QuestionData[]> => {
  const result = await http<QuestionDataFromServer[]>({
    path: `questions?search=${criteria}`,
  });
  if (result.ok && result.body) {
    return result.body.map(mapQuestionFromServer);
  } else {
    return [];
  }
};

export const postQuestion = async (
  question: PostQuestionData,
): Promise<QuestionData | undefined> => {
  const accessToken = await getAccessToken();
  const result = await http<QuestionDataFromServer, PostQuestionData>({
    path: 'questions',
    method: 'post',
    body: question,
    accessToken,
  });
  if (result.ok && result.body) {
    return mapQuestionFromServer(result.body);
  } else {
    return undefined;
  }
};

export interface PostAnswerData {
  questionId: number;
  content: string;
  userName: string;
  created: Date;
}
export const postAnswer = async (
  answer: PostAnswerData,
): Promise<AnswerData | undefined> => {
  const accessToken = await getAccessToken();
  const result = await http<AnswerData, PostAnswerData>({
    path: 'questions/answer',
    method: 'post',
    body: answer,
    accessToken,
  });
  if (result.ok) {
    return result.body;
  } else {
    return undefined;
  }
};
