import * as signalR from "@microsoft/signalr";

export default class HubConnector {
  public readonly connection: signalR.HubConnection;

  constructor(baseApiRoute: string, hubRoute: string) {
    const fullRoute = "https://localhost:6010/api" /*baseApiRoute*/ + "/" + hubRoute;
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(fullRoute, {
        transport: signalR.HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect()
      .build();
  }
}
