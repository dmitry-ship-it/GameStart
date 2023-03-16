import axios, { AxiosRequestConfig } from "axios";

export default class ApiRouter {
  private baseUrl = "https://localhost:6001/api";

  public static account = new ApiRouter("account");
  public static inventory = new ApiRouter("inventory");
  public static catalog = new ApiRouter("catalog");
  public static order = new ApiRouter("order");
  public static address = new ApiRouter("address");

  private constructor(urlPart: string | null = null) {
    if (urlPart !== null) this.baseUrl += "/" + urlPart;
  }

  public async get<T>(relativeRoute: string, addSlash: boolean = true) {
    return await this.axiosBodyCall<T>(this.baseUrl + (addSlash ? "/" : "") + relativeRoute, null, "GET");
  }

  public async post<T>(relativeRoute: string, body: T, addSlash: boolean = true) {
    return await this.axiosBodyCall(this.baseUrl + (addSlash ? "/" : "") + relativeRoute, body, "POST");
  }

  public async put<T>(relativeRoute: string, body: T, addSlash: boolean = true) {
    return await this.axiosBodyCall(this.baseUrl + (addSlash ? "/" : "") + relativeRoute, body, "PUT");
  }

  public async delete(relativeRoute: string, addSlash: boolean = true) {
    return await this.axiosBodyCall(this.baseUrl + (addSlash ? "/" : "") + relativeRoute, null, "DELETE");
  }

  private async axiosBodyCall<T>(fullPath: string, data: T | null, method: string) {
    const config: AxiosRequestConfig = {
      method: method,
      headers: {
        "Access-Control-Allow-Origin": "https://localhost:3000",
      },
      withCredentials: true,
      maxRedirects: 5,
    };

    if (data !== null) {
      config.data = data;
    }

    return await axios<T>(fullPath, config);
  }
}
