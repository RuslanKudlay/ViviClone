import { Injectable } from '@angular/core';
import { PostModel } from '../models/post.model';
import { GenericRestService } from './generic.service.';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class BlogService extends  GenericRestService<PostModel> {
  constructor(protected http: HttpClient) {
    super(http, 'api/Post');   
  }
}
