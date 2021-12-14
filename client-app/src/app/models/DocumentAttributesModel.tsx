import { AttributesModel } from "./AttributesModel";

export interface DocumentAttributesModel {
    docId: number;
    name: string;
    models: AttributesModel[];
}