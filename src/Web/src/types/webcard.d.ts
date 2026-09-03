export {};

declare global {
  interface Navigator {
    webcard: WebCard;
  }

  interface WebCard {
    readers(): Promise<WebCardReader[]>;

    cardInserted?: (reader: WebCardReader) => void;
    cardRemoved?: (reader: WebCardReader) => void;

    readersConnected?: (count: number) => void;
    readersDisconnected?: (count: number) => void;
  }

  interface WebCardReader {
    name: string;
    atr: string;

    connect(shared?: boolean): Promise<void>;

    transceive(apdu: string): Promise<string>;

    disconnect(): Promise<void>;
  }
}