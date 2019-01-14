
import { Component } from "@angular/core";
import { EsolangService } from './esolang.service';
// import { LocalStorage } from 'angular2-extensible-decorators/components';

@Component({
    selector: "execution",
    template: `
            <form (ngSubmit)="run()" #executionForm="ngForm">
            <div class="row">
                <div class="col-12">
                    <h4>Source code</h4>
                </div>
</div>
<div class="row">
                <div class="col-12">
                    <textarea cols="80" rows="10" [(ngModel)]="sourceCode" style="min-width: 100%;" 
                     name="sourceCode" required [disabled]="running"></textarea>
                </div>
</div>
            <p></p>
            <div class="row">
                <div class="col-12">
                    <button type="submit" class="btn btn-primary" [disabled]="!executionForm.form.valid || running">
                      Run
                    </button>&nbsp;    
                    <button type="button" class="btn btn-danger" (click)="cancel()" [disabled]="!running">
                      Cancel
                    </button>    
                </div>
            </div> 
            
            <div class="row" *ngIf="inputRequired">
                <div class="col-12">
                    <h4>Input</h4>
                </div>
                <div class="col-12">
                    <input [(ngModel)]="programInput" name="programInput" required/>&nbsp;
                    <button type="button" class="btn btn-primary" (click)="send()" 
                        [disabled]="!executionForm.form.valid">
                    Send
                    </button>
                </div>
            </div>
            </form>
            <div class="row">
                <div class="col-12">
                    <h4>Output</h4>
                </div>
</div>
<div class="row">
                <div class="col-12">
                    <textarea cols="80" rows="10" [value]="programOutput" disabled style="min-width: 100%;"></textarea>
                </div>
            </div>
    `,
    providers: [EsolangService]
})

export class ExecutionComponent {
    language: string;
    sourceCode: string;
    programOutput = '';
    programInput = '';
    running = false; 
    inputRequired = false;

    constructor(private _esolangService: EsolangService) {
        console.log('built EC');
    }

    changeLanguage(lang) {
        this.language = lang;
        console.log('Language changed to ' + this.language);
    }

    run() {
        console.log('Run ' + this.language + ' program --> ' + this.sourceCode);
        this.running = true;
        this.programOutput = '';
        this._esolangService.execute(
            {
                language: this.language,
                source: this.sourceCode
            },
            {
                next: m => this.programOutput += m,
                complete: () => this.cancel()
            },
            () => this.inputRequired = true
        );
    } 

    send() {
        console.log('Sending ' + this.programInput);
        this._esolangService.send(this.programInput);
        this.inputRequired = false;
    }

    cancel() {
        this.running = this.inputRequired = false;
        this._esolangService.close();
    } 

}
