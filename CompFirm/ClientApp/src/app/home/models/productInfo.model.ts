import { CharacteristicValue } from "./characteristicValue.model";
import { Group } from "./group.model";
import { Manufacturer } from "./manufacturer.model";

export interface ProductInfo {
  productId?: number;
  name: string;
  price?: number;
  imageUrl?: string;
  group?: Group;
  manufacturer?: Manufacturer;
  productType?: string;
  characteristics: CharacteristicValue[];
}
