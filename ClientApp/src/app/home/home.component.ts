import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ReturnObj } from '../models/returnObj.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  private conn: HubConnection;
  private username: string;
  connId: string = 'test';

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

    this.conn.on("userAdded", this.onUserAdded.bind(this))
    this.conn.on("connected", this.onConnected.bind(this))

    this.conn.start()
    .then((resp) => {
      console.log('connection established')
    })
    .catch(err => {
      console.log(err)
    })
  }

  invokeEvent() {
    console.log(this.connId)
    this.http.get(this.baseUrl + 'SignalRConnection/' + this.username).subscribe(result => {
      console.log(result)
    }, error => console.error(error));
  }

  invokeErrorEvent() {
    this.http.get(this.baseUrl + 'SignalRConnection/InvokeError/' + this.username).subscribe(result => {
      console.log(result)
    }, error => console.error(error));
  }

  onUserAdded(returnObj: ReturnObj) {
    if (returnObj.TriggeringUser.toLowerCase() !== this.username.toLowerCase()) {
      alert(`${returnObj.TriggeringUser} added to group`)
    } else {
      console.log('came from same user')
    }
  }

  onConnected(id) {
    console.log('connection id', id)
    this.connId = id;
  }
}
