import axios, { AxiosRequestConfig } from "axios";
import HubConnector from "./SignalRConnector";

export default class ApiRouter {
  private static readonly baseUrl = "https://localhost:6001/api";
  private route = ApiRouter.baseUrl;

  public static readonly account = new ApiRouter("account");
  public static readonly inventory = new ApiRouter("inventory");
  public static readonly catalog = new ApiRouter("catalog");
  public static readonly order = new ApiRouter("order");
  public static readonly address = new ApiRouter("address");

  public static createSignalRConnection(hubRoute: string) {
    return new HubConnector(this.baseUrl, hubRoute).connection;
  }

  private constructor(urlPart: string | null = null) {
    if (urlPart !== null) this.route += "/" + urlPart;
  }

  public async get<T>(relativeRoute: string, addSlash: boolean = true) {
    return await this.axiosCall<T>(this.route + (addSlash ? "/" : "") + relativeRoute, null, "GET");
  }

  public async post<T>(relativeRoute: string, body: T, addSlash: boolean = true) {
    return await this.axiosCall(this.route + (addSlash ? "/" : "") + relativeRoute, body, "POST");
  }

  public async put<T>(relativeRoute: string, body: T, addSlash: boolean = true) {
    return await this.axiosCall(this.route + (addSlash ? "/" : "") + relativeRoute, body, "PUT");
  }

  public async delete(relativeRoute: string, addSlash: boolean = true) {
    return await this.axiosCall(this.route + (addSlash ? "/" : "") + relativeRoute, null, "DELETE");
  }

  private async axiosCall<T>(fullPath: string, data: T | null, method: string) {
    const config: AxiosRequestConfig = {
      method: method,
      headers: {
        "Access-Control-Allow-Origin": "https://localhost:3000",
      },
      withCredentials: true,
      maxRedirects: 5,
      validateStatus: () => true,
    };

    if (data !== null) {
      config.data = data;
    }

    return await axios<T>(fullPath, config);
  }
}
