export interface GetAllTransactionsResponse {
    id: number;
    type: number;
    nature: string;
    value: number;
    signedValue: number;
    cpf: string;
    card: string; 
    occurredAt: Date | string;
    storeName: string;
    storeOwner: string;
}



