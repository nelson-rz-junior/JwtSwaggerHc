import { Component, OnInit } from '@angular/core';
import { ApiNotification } from '../models/ApiNotification';

// Install SignalR Client Library
// Inside ClientApp folder
// PM> npm i @microsoft/signalr
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-api-notification-component',
  templateUrl: './api-notification.component.html',
  styleUrls: ['./api-notification.component.css']
})
export class ApiNotificationComponent implements OnInit {
  public messages = Array<ApiNotification>();

  constructor() { }

  ngOnInit(): void {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:44352/hub')
      .withAutomaticReconnect()
      .build();

    connection.on('messageReceived', (apiMethod: string, message: string) => {
      this.messages.push({ apiMethod, message });
    });

    connection.start()
      .then(() => {
        const apiMethod = 'JwtSwaggerHc API';
        const message = 'Connected';

        this.messages.push({ apiMethod, message });
      })
      .catch(err => console.error(err));
  }
}
