import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Announcement } from '../../../models/announcement.model';
import { AnnouncementService } from '../../../services/announcement.service';
import { CreateCyrillicFriendlySuburlService } from '../../../services/create-cyrillic-friendly-suburl.service';

@Component({
    templateUrl: './add-edit-announcement.component.html'
})
export class AddEditAnnouncementComponent implements OnInit {

    public myForm: FormGroup;
    private subUrl: string;
    public announcement: Announcement = new Announcement();
    private status: string;
    public titleStatus: string;
    private bodyText: string;

    constructor(
        private announcementService: AnnouncementService,
        private route: ActivatedRoute,
        private formBuilder: FormBuilder,
        private location: Location,
        private createCyrillicFriendlySuburlService: CreateCyrillicFriendlySuburlService
    ) { }

    public ngOnInit(): void {
        this.initForm();
        this.route.params.subscribe(params => {
            this.subUrl = params['subUrl'];
            if (this.subUrl && this.subUrl != 'newAnnouncement') {
                this.titleStatus = 'Update Announcement';
                this.announcementService.GetBySubUrl(this.subUrl)
                    .subscribe((response: Announcement) => {
                        this.announcement = response;
                        this.initForm(this.announcement);
                    }, (error: any) => {
                        this.status = 'error ' + error;
                    });
            } else {
                this.titleStatus = 'Add Announcement';
            }
        });
    }

    private initForm(announcement?: Announcement): void {
        if (announcement) {
            this.myForm = this.formBuilder.group({
                "title": [announcement.title, [Validators.required]],
                "subUrl": [announcement.subUrl, [Validators.required]],
                "isEnable": [announcement.isEnable, []],
                "date": [announcement.date, []],
                "lastUpdateDate": [announcement.lastUpdateDate, []],
                "metaDescription": [announcement.metaDescription, []],
                "metaKeywords": [announcement.metaKeywords, []]
            });
        } else {
            this.myForm = this.formBuilder.group({
                "title": ['', [Validators.required]],
                "subUrl": ['', [Validators.required]],
                "isEnable": [false, []],
                "date": [new Date(), []],
                "lastUpdateDate": [new Date(), []],
                "metaDescription": ['', []],
                "metaKeywords": ['', []]
            });
        }
    }

    public addAnnouncement(): void {
        this.setAnnouncementByForm(this.announcement, this.myForm);
        this.announcementService.Save(this.announcement).subscribe((response: any) => {
          this.status = 'Added';
          this.location.back();
        }, (error: any) => {
          this.status = 'error ' + error;
        });
      }

    public updateAnnouncement(): void {
        this.setAnnouncementByForm(this.announcement, this.myForm);
        this.announcementService.Update(this.announcement).subscribe(response => {
              this.status = 'Updated';
              this.location.back();
        });
    }

    public onKeyAnnouncementTitle(newValue: string) {
        this.myForm.controls['subUrl'].setValue(this.createCyrillicFriendlySuburlService.createSuburl(newValue));
    }

    public announcementBodyUpdated(newValue: string): void {
        this.bodyText = newValue;
    }

    private setAnnouncementByForm(announcement: Announcement, form: FormGroup): void {
        announcement.title = form.value.title;
        announcement.subUrl = form.value.subUrl;
        announcement.isEnable = form.value.isEnable;
        announcement.date = form.value.date;
        announcement.lastUpdateDate = form.value.lastUpdateDate;
        announcement.metaDescription = form.value.metaDescription;
        announcement.metaKeywords = form.value.metaKeywords;
        if(this.bodyText)
            announcement.body = this.bodyText;
    }

    public errorDisplayed(controlName: string): boolean {
        return this.myForm.controls[controlName].invalid && this.myForm.controls[controlName].touched;
    }
}
