import { TestBed, inject } from '@angular/core/testing';

import { UserService } from './user.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { Room } from './rooms/room.model';

let sut: UserService;

describe('UserService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, ReactiveFormsModule, BrowserModule,
        HttpClientModule],
      providers: [UserService]
    });
    sut = new UserService(null, null);
  });

  it('should be created', inject([UserService], (service: UserService) => {
    expect(service).toBeTruthy();
  }));
  it('should', () => {
    const room = new Room('1', '1');
    sessionStorage.setItem('UserName', 'name');
    sut.addRoom(new Room('1', '1'));
    expect(sessionStorage.getItem('UserName')).toEqual(room.creatorName);
  });
});
