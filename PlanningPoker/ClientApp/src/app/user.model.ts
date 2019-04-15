export class User {
    constructor(name: string, password: string) {
      this.Password = password;
      this.Name = name;
    }
    Password: string;
    Name: string;
  }
