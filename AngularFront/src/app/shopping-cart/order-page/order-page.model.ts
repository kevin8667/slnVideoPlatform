export interface OrderPage {
  orderId: number;
  shoppingCartId: number;
  memberId?: number;
  planId?: number;
  planName?: string;
  videoId?: number;
  videoName?: string;
  couponId: number;
  couponName?: string;
  orderDate: Date;
  orderTotalPrice: number;
  deliveryName: string;
  deliveryAddress: string;
  paymentStatus: number;
  deliveryStatus: number;
  payments?: string;
  deliverys?: string;
}
