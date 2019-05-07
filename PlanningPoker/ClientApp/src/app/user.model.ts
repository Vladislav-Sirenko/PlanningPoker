export class User {
  Id: number;
  name: string;
  password: string;
  email: string;
  ConnectionId: string;
  Vote: number;
  RoomId: string;
  constructor(name: string, password: string, email: string) {
    this.password = password;
    this.name = name;
    this.email = email;
  }
}
