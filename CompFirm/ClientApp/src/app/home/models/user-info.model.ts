export interface UserInfo {
  login: string;
  surname: string;
  name: string;
  patronymic: string;
  phone: string;
  isAdmin: boolean;
  roles: string[];
}

export interface UpdateUserRequest extends UserInfo {
  password?: string;
  newPassword?: string;
}
