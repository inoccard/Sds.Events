import { EventService } from './../services/event/event.service';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {
  // tslint:disable-next-line: variable-name
  _filterList: string;

  get filterList(): string {
    return this._filterList;
  }
  set filterList(value: string) {
    this._filterList = value;
    this.eventsFiltered = this._filterList ? this.filterEvent(this._filterList) : this.events;
  }
  eventsFiltered: any = [];
  events: any = [];
  showImg = false;

  constructor(private eventService: EventService) { }
  ngOnInit() {
    this.getEvents();
  }

  filterEvent(filterBy: string): any {
    filterBy = filterBy.toLocaleLowerCase();
    return this.events.filter(
      (      event: { theme: string; }) => event.theme.toLocaleLowerCase().indexOf(filterBy) !== -1
    );
  }

  alterImg() {
    this.showImg = !this.showImg;
  }
  getEvents() {
    this.eventService.getEvents().subscribe(
    response => {
      this.events = response;
    }, error => {
      console.log(error);
    }
    );
  }
}
