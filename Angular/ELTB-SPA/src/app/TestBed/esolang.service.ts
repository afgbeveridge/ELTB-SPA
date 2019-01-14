import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NextObserver, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SourceBundle } from './sourcebundle.interface';
import { LanguageDescription } from './languageDescription.interface';
import { $WebSocket } from 'angular2-websocket/angular2-websocket';

@Injectable()
export class EsolangService {

  private static _interruptSequence = '\t';
  private static _targetUrl = "api/EsotericLanguage/";
  private _channel: $WebSocket;

  constructor(private _http: HttpClient) {
    console.log('built ELS - ok' + this._http);
  }

  supportedLanguages() {
    return this
      ._http
      .get(this.formUrl(false))
      .pipe(map((data: any[]) => {
        return <LanguageDescription[]>data
      }));
  }

  execute(bundle: SourceBundle, handler: NextObserver<any>, interrupt: () => void) {
    this.close();
    this._channel = new $WebSocket(this.formUrl(true));
    this._channel.connect();
    this.send('|' + bundle.language + '|' + bundle.source);
    this._channel
      .getDataStream()
      .subscribe(m => {
        var received = m['data'];
        if (!received || received[0] != EsolangService._interruptSequence) {
          console.log(received);
          handler.next(received);
        }
        else
          interrupt();
      },
        e => console.log('WS error: ' + e),
        () => {
          console.log('Seems like the subject is exhausted');
          handler.complete();
        }
      );
  }

  send(text: string) {
    this._channel.send(text).subscribe();
  }

  close() {
    if (this._channel) {
      this._channel.close(true);
      this._channel = null;
      console.log('Closed ws');
    }
  }

  private formUrl(webSocketRequest: boolean) {
    return (webSocketRequest ? "ws" : "http") + "://localhost:55444" + "/" + EsolangService._targetUrl + (webSocketRequest ? "execute" : "SupportedLanguages");
  }

}
