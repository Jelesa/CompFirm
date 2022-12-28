export interface ProductsFilter {
  name: string;
  groupId: number;
  productType: string;
  minPrice: number;
  maxPrice: number;
  limit?: number;
  offset?: number;
}
