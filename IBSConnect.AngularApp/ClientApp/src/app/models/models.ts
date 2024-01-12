export interface IUser {
  id: number;
  userName: string;
  firstName: string;
  lastName: string;
  role: string;
  password: string;
  isActive: boolean;
  isInUse: boolean;
}

export interface IMember {
  id?: number;
  idNo?: string;
  firstName: string;
  middleName: string;
  lastName: string;
  password?: string;
  isActive?: boolean;
  isInUse?: boolean;
  picture?: string;

  credits: number;
  totalMinutes: number;
  remainingMinutes: number;
}

export interface IMemberBill {
  memberId: number;
  idNo: string;
  firstName: string;
  middleName: string;
  lastName: string;
  allottedTime: number;
  paidMinutes: number;
  totalMinutes: number;
  excessMinutes: number;
  rate: number;
  charge: number;
}

export interface IAuthenticatedUser {
  id: number;
  username: string;
  fullname: string;
  role: string;
  passExpiring: boolean;
  passExpired: boolean;
  daysLeft: number;
  passChanged: Date;
  token?: string;
}



export interface IValidationFailure {
  property: string;
  message: string;
}


export interface IFilter {
  page: number;
  pageSize?: number;
  orderBy?: string;
  sortOrder?: string;
  query?: string;
  filterColumns?: string[];
}

export interface IQueryResult<T> {
  count: number;
  result: T[];
  columnValues?: Record<string, string[]>;
}

export interface Item {
  id: number;
  name: string;
}

export interface CategoryItem {
  id: number;
  name: string;
  isFreeTier: boolean;
}

export interface FilterRequest {
  query?: string;
  pageSize: number;
  page: number;
  orderBy?: string;
  sortOrder?: string;

}

export interface ISession {
  id: number;
  name: string;
  startTime: string;
  endTime: string;
  totalMinutes: number;
}

export interface ISetting {
  id: number;
  name: string;
  value: string;
}


export interface ICurrentSession {
  id: number;
  idNo: number;
  firstName: string;
  lastName: string;
  middleName: string;
  age: number;
  category: string;
  college: string;
  course: string;
  section: string;
  year: string;
  picture: string;
  startTime: string;
  endTime: string;
  totalMinutes: number;
  remainingTime: number;
  timeAllotted: number;
  notes: string;
}


export interface ImportResult {
  added: number;
  updated: number;
  errors: string[];
}

export interface IPayment {
  id: number;
  minutes: number;
  rate: number;
  amount: number;
  createdTime: string;
}

export interface IPaymentArrears {
  id: number;
  minutes: number;
  rate: number;
  amount: number;
  createdTime: string;
}

export interface IIBSTranHistory {
  id: number;
  minutes: number;
  rate: number;
  amount: number;
  createdTime: string;
  memberId: number;
  idNo: string;
  firstName: string;
  middleName: string;
  lastName: string;
  allottedTime:  number;
  totalMinutes :  number;
  paidMinutes :  number;
  timeLeft :  number;
  excessMinutes :  number;
  sySemester :  string;
}
