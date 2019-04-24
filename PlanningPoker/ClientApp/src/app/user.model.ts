export class User {
  Id: number;
  Name: string;
  Password: string;
  Email: string;
  ConnectionId: string;
  Vote: number;
  RoomId: string;
  constructor(name: string, password: string, email: string) {
    this.Password = password;
    this.Name = name;
    this.Email = email;
  }
}
