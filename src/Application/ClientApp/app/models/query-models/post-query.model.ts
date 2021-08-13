import { PostModel } from '../post.model';
import { QueryBaseModel } from './base-query.model';

export class PostQueryModel extends QueryBaseModel<PostModel> {
    public titleContains: string;
}
