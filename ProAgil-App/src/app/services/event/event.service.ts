import { Events } from './../../models/Events';
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
  getEventByTheme(theme: string): Observable<Events[]>{
    return this.http.get<Events[]>(`${this.baseURL}/getByTheme/${theme}`);
  }
  getEvent(id: number): Observable<Events>{
    return this.http.get<Events>(`${this.baseURL}/get/${id}`);
  }
  saveEvent(event: Events) {
    if (!event.id || event.id === 0) {
      return this.http.post(this.baseURL, event);
    } else {
      return this.http.put(`${this.baseURL}/${event.id}`, event);
    }
  }
  deleteEvent(id: number) {
    return this.http.delete(`${this.baseURL}/${id}`);
  }

  /**
   * upload de imagens
   * @param file
   */
  postUpload(file: File, name: string) {
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload, name);
    return this.http.post(`${this.baseURL}/upload`, formData);
  }
}
