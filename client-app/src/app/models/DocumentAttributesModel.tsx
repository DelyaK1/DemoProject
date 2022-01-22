import { AttributesModel } from "./AttributesModel";

export interface DocumentAttributesModel {
    pageId: number;
    name: string;
    attributes: AttributesModel;
}