import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FileHelperServiceService {

  constructor() { }

  public saveStringToFile(filename: string, content: string) {
    var pom = document.createElement("a");
    pom.setAttribute("href", "data:text/plain;charset=utf-8," + encodeURIComponent(content));
    pom.setAttribute("download", filename);
    pom.click();
  }
}
