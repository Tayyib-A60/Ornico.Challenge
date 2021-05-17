export interface User {
    id: number;
    name: string;
    email: string;
    password: string;
    role: Role;
}

export interface UserToLogin {
    email: string;
    password: string;
    isAdmin: boolean;
}

export enum Role
{
    Reader = 1,
    Author = 2
}
