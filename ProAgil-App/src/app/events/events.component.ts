import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {

  events: any = [
    {
      eventId: 1,
      theme: 'Angular',
      local: 'Hortolândia'
    },
    {
      eventId: 2,
      theme: '.NET Core',
      local: 'Campinas'
    },
    {
      eventId: 2,
      theme: 'Agular e .NET Core',
      local: 'Sumaré'
    }
  ];
  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  getEvents() {
    this.http.get('http://localhost:5000/api/values').subscribe(
      response => {
        this.events = response;
      }, error => {
        console.log(error);
      }
    );
  }

}
