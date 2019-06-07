﻿using Prism.Commands;
using Prism.Events;
using Purchase.Model;
using Purchase.UI.Data.Repositories;
using Purchase.UI.View.Services;
using Purchase.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.UI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private IMeetingRepository _meetingRepository;
        private IMessageDialogService _messageDialogService;
        private MeetingWrapper _meeting;

        public MeetingDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository) : base(eventAggregator)
        {
            _meetingRepository = meetingRepository;
            _messageDialogService = messageDialogService;
        }


        public MeetingWrapper Meeting
        {
            get { return _meeting;}
            private set
            {
                _meeting = value;
                OnpropertyChanged();
            }
        }

        public override async Task LoadAsync(int? meetingId)
        {
            var meeting = meetingId.HasValue ? await _meetingRepository.GetByIdAsync(meetingId.Value) : CreateNewMeeting();

            InitializeMeeting(meeting);
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _meetingRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }

        protected async override void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete this meeting {Meeting.Title}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                _meetingRepository.Remove(Meeting.Model);
                await _meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected async override void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }
    }
}
