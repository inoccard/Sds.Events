
export class ErrorDetails {
  title: string;
  status: number;
  messages: string[];

  constructor(title: string, status: number, messages: string[]) {
    this.title = title;
    this.status = status;
    this.messages = messages;
  }
}
