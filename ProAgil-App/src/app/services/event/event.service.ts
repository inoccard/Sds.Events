import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  events: any = [];
  baseURL = 'http://localhost:5000/event';
  constructor(private http: HttpClient) {
    this.getEvents();
  }
  getEvents() {
    return this.http.get(this.baseURL);
  }
}
