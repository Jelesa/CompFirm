export interface RequestCard {
  number: string;
  creationDate: string;
  periodExecution: string;
  status: string;
  userName: string;
  sum?: number;
  requestItems: RequestItem[];
  journal: RequestJournalItem[];
}

export interface RequestItem {
  productId: number;
  productName: string;
  productImage: string;
  productPrice: number;
  count: number;
}

export interface RequestJournalItem {
  statusName: string;
  statusDate: string;
}
