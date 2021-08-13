import { Announcement } from './../announcement.model';
import { QueryBaseModel } from './base-query.model';

export class AnnouncementQueryModel extends QueryBaseModel<Announcement> {
    public TitleContains: string = '';
    public BodyContains: string = '';
}
