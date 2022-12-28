import { BaseSearchFilter } from "../../shared/models/base-search-filter.model";

export interface ProductsMovingFilter extends BaseSearchFilter {
  movingType: string;
  dateFrom: string;
  dateTo: string;
}
