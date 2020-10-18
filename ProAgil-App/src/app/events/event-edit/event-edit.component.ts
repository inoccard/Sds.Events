import { Events } from './../../models/Events';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { EventService } from 'src/app/services/event/event.service';

@Component({
  selector: 'app-event-edit',
  templateUrl: './event-edit.component.html',
  styleUrls: ['./event-edit.component.css']
})
export class EventEditComponent implements OnInit {

  title = 'Editar Evento';
  event: Events;
  registerForm: FormGroup;

  constructor(
    private eventService: EventService,
    private modalService: BsModalService,
    private fb: FormBuilder,
//    private localeService: BsLocaleService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.validation();
  }

  validation() {
    this.registerForm = this.fb.group({
      theme: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      eventDate: ['', Validators.required],
      personQtd: ['', [Validators.required, Validators.max(120000), Validators.pattern(/^-?(0|[1-9]\d*)?$/)]],
      imageURL: ['', Validators.required],
      contactPhone: ['', Validators.required],
      contactEmail: ['', [Validators.required, Validators.email]]
    });
  }

  onFileChange(data: any) {}

}
