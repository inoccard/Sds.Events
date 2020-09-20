import { Events } from './../../models/Events';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  events: any = [];
  baseURL = 'http://localhost:5000/event';
  tokenHeader: HttpHeaders;

  constructor(private http: HttpClient) {
    this.tokenHeader = new HttpHeaders({ Authorization: `Bearer ${localStorage.getItem('token')}` });
    this.getEvents();
  }
  getEvents() {
    return this.http.get(this.baseURL, { headers: this.tokenHeader });
  }
  getEventByTheme(theme: string): Observable<Events[]>{
    return this.http.get<Events[]>(`${this.baseURL}/getByTheme/${theme}`);
  }
  getEvent(id: number): Observable<Events>{
    return this.http.get<Events>(`${this.baseURL}/get/${id}`);
  }
  saveEvent(event: Events) {
    if (!event.id || event.id === 0) {
      return this.http.post(this.baseURL, event, { headers: this.tokenHeader });
    } else {
      return this.http.put(`${this.baseURL}/${event.id}`, event, { headers: this.tokenHeader });
    }
  }
  deleteEvent(id: number) {
    return this.http.delete(`${this.baseURL}/${id}`);
  }

  /**
   * upload de imagens
   */
  postUpload(file: File, name: string) {
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload, name);
    return this.http.post(`${this.baseURL}/upload`, formData, { headers: this.tokenHeader });
  }
}
