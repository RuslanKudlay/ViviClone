import { Injectable } from '@angular/core';
import { Announcement } from '../models/announcement.model';
import { GenericRestService } from './generic.service.';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AnnouncementService extends  GenericRestService<Announcement> {
  constructor(protected http: HttpClient) {
    super(http, 'api/Announcement');   
  }
}
