import axios, { AxiosRequestConfig } from "axios";

export default class ApiRouter {
  private baseUrl = "https://localhost:6001/api";

  public static readonly account = new ApiRouter("account");
  public static readonly inventory = new ApiRouter("inventory");
  public static readonly catalog = new ApiRouter("catalog");
  public static readonly order = new ApiRouter("order");
  public static readonly address = new ApiRouter("address");

  private constructor(urlPart: string | null = null) {
    if (urlPart !== null) this.baseUrl += "/" + urlPart;
  }

  public async get<T>(relativeRoute: string, addSlash: boolean = true) {
    return await this.axiosCall<T>(this.baseUrl + (addSlash ? "/" : "") + relativeRoute, null, "GET");
  }

  public async post<T>(relativeRoute: string, body: T, addSlash: boolean = true) {
    return await this.axiosCall(this.baseUrl + (addSlash ? "/" : "") + relativeRoute, body, "POST");
  }

  public async put<T>(relativeRoute: string, body: T, addSlash: boolean = true) {
    return await this.axiosCall(this.baseUrl + (addSlash ? "/" : "") + relativeRoute, body, "PUT");
  }

  public async delete(relativeRoute: string, addSlash: boolean = true) {
    return await this.axiosCall(this.baseUrl + (addSlash ? "/" : "") + relativeRoute, null, "DELETE");
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
