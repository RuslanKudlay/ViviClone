export enum UserStatus
{
    Active,
    Disabled,
    Blocked
}

export class UserModel {
    id: number;
    email: string;
    phone: string;
    firstName: string;
    lastName: string;
    createdDate: Date;
    name: string;
    role: string;
    status: UserStatus;
}
