import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { CardComponent } from './card/card.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HeaderComponent } from './header/header.component';
import { RoomComponent } from './room/room.component';
import { UserService } from './user.service';
import { RoomsComponent } from './rooms/rooms.component';
import { InjectionToken } from '@angular/core';
import { MessageComponentModule } from './message/message.module';
export const BASE_URL = new InjectionToken<string>('BASE_URL');

@NgModule({
  declarations: [
    AppComponent,
    CardComponent,
    DashboardComponent,
    HeaderComponent,
    RoomComponent,
    MessageComponentModule,
    RoomsComponent,
    ReactiveFormsModule
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: 'app', component: AppComponent },
      {
        path: 'room/:id', component: RoomComponent,
        data: {
          type: 'edit'
        }
      }
    ])
  ],
  providers: [UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
