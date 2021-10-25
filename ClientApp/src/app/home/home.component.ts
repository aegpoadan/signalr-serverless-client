import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  private conn: HubConnection;
  private username: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.username = Math.floor(Math.random() * 10000) + '@gmail.com';

    this.conn = new HubConnectionBuilder()
      .withUrl("http://localhost:7071/api", {
        headers: {
          "x-ms-signalr-user-id": this.username
        }
      })
      .build();
    console.log('connecting...')

    this.conn.on("userAdded", this.onUserAdded)
    this.conn.on("connected", this.onConnected)

    this.conn.start()
    .then((resp) => {
      console.log('connection established')
    })
    .catch(err => {
      console.log(err)
    })
  }

  invokeEvent() {
    this.http.get(this.baseUrl + 'SignalRConnection/' + this.username).subscribe(result => {
      console.log(result)
    }, error => console.error(error));
  }

  invokeErrorEvent() {
    this.http.get(this.baseUrl + 'SignalRConnection/InvokeError/' + this.username).subscribe(result => {
      console.log(result)
    }, error => console.error(error));
  }

  onUserAdded(userId) {
    console.log('user added', userId)
  }

  onConnected(connectionId) {
    console.log('connection id', connectionId)
  }
}
