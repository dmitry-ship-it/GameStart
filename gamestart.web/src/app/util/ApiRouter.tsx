import axios, { AxiosRequestConfig } from "axios";

export default class ApiRouter {
  private static readonly baseUrl = "https://localhost:6001/api/";

  public static async get<T>(relativeRoute: string) {
    return await this.axiosBodyCall<T>(this.baseUrl + relativeRoute, null, "GET");
  }

  public static async post<T>(relativeRoute: string, body: T) {
    return await this.axiosBodyCall(this.baseUrl + relativeRoute, body, "POST");
  }

  public static async put<T>(relativeRoute: string, body: T) {
    return await this.axiosBodyCall(this.baseUrl + relativeRoute, body, "PUT");
  }

  public static async delete(relativeRoute: string) {
    return await this.axiosBodyCall(this.baseUrl + relativeRoute, null, "DELETE");
  }

  private static async axiosBodyCall<T>(fullPath: string, data: T | null, method: string) {
    const config: AxiosRequestConfig = {
      method: method,
      headers: {
        Accept: "application/json; charset=utf-8",
      },
    };

    if (data !== null) config.data = data;

    return await axios<T>(fullPath, config);
  }
}
