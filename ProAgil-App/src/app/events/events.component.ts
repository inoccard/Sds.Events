import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {

  events: any = [];
  showImg = false;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getEvents();
  }

  alterImg() {
    this.showImg = !this.showImg;
  }

  getEvents() {
    this.http.get('http://localhost:5000/event').subscribe(
      response => {
        this.events = response;
      }, error => {
        console.log(error);
      }
    );
  }

}
