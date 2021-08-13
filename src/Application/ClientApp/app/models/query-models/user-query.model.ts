import { UserModel } from '../user-model';
import { QueryBaseModel } from './base-query.model';

export class UserQueryModel extends QueryBaseModel<UserModel> {
    public emailContains: string = '';
}
