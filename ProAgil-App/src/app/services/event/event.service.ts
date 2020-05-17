import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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
  getEventByTheme(theme: string): Observable<Event[]>{
    return this.http.get<Event[]>(`${this.baseURL}/getByTheme/${theme}`);
  }
  getEvent(id: number): Observable<Event>{
    return this.http.get<Event>(`${this.baseURL}/get/${id}`);
  }
  saveEvent(event: Event){
    return this.http.post(this.baseURL, event);
  }
}
