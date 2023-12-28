import { Events } from './../../models/Events';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  baseURL = `${environment.apiEventBaseUrl}event/`;

  constructor(private http: HttpClient) {
    this.getEvents();
  }

  getEvents() {
    console.log(this.baseURL)
    return this.http.get(`${this.baseURL}events`);
  }

  getEventByTheme(theme: string): Observable<Events[]>{
    return this.http.get<Events[]>(`${this.baseURL}get-by-theme/${theme}`);
  }

  getEvent(id: number): Observable<Events>{
    return this.http.get<Events>(`${this.baseURL}${id}`);
  }

  saveEvent(event: Events) {
    if (!event.id || event.id === 0) {
      return this.http.post(this.baseURL, event);
    } else {
      return this.http.put(`${this.baseURL}${event.id}`, event);
    }
  }

  deleteEvent(id: number) {
    return this.http.delete(`${this.baseURL}${id}`);
  }

  /**
   * upload de imagens
   */
  postUpload(file: FileList, name: string) {
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload, name);
    return this.http.post(`${this.baseURL}upload`, formData);
  }
}
