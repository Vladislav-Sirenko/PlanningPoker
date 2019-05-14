export class User {
  id: number;
  name: string;
  password: string;
  email: string;
  connectionId: string;
  vote: number;
  roomId: string;
  constructor(name: string, password: string, email: string) {
    this.password = password;
    this.name = name;
    this.email = email;
  }
}
