﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ColorCode.Common;
using CSharpFunctionalExtensions;
using ElkaUWP.DataLayer.Propertiary.Abstractions.Bases;
using ElkaUWP.DataLayer.Propertiary.Converters;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Propertiary.Services;
using Nito.Mvvm;
using Prism.Mvvm;
using Prism.Navigation;

namespace ElkaUWP.Modularity.GradesModule.ViewModels
{
    public class FinalGradesViewModel : BindableBase, INavigatedAware
    {
        private FinalGradesService _finalGradesService;
        private PartialGradesService _partialGradesService;
        private INavigationService _navigationService;

        public ObservableCollection<SubjectApproach> SubjectApproaches;
        public ObservableCollection<PartialGradeNode> UsosPartialGradesSource;

        private SubjectApproach _selectedSubjectApproach;
        private PartialGradesModel _partialGrades;

        public SubjectApproach SelectedSubjectApproach
        {
            get => _selectedSubjectApproach;
            set => SetProperty(storage: ref _selectedSubjectApproach, value: value, propertyName: nameof(SelectedSubjectApproach));
        }

        public PartialGradesModel PartialGrades
        {
            get => _partialGrades;
            set => SetProperty(storage: ref _partialGrades, value: value, propertyName: nameof(PartialGrades));
        }

        public NotifyTask MasterPaneInitilizationTaskNotifier { get; private set; }
        public NotifyTask<PartialGradesModel> DetailPaneInitilizationTaskNotifier { get; private set; }

        public FinalGradesViewModel(FinalGradesService finalGradesService, PartialGradesService partialGradesService)
        {
            _finalGradesService = finalGradesService;
            _partialGradesService = partialGradesService;
            SubjectApproaches = new ObservableCollection<SubjectApproach>();
            UsosPartialGradesSource = new ObservableCollection<PartialGradeNode>();
            MasterPaneInitilizationTaskNotifier = NotifyTask.Create(asyncAction: LoadMasterElementsAsync);
        }

        /// <inheritdoc />
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        /// <inheritdoc />
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _navigationService = parameters.GetNavigationService();
        }

        private async Task LoadMasterElementsAsync()
        {
            var subjectApproaches = await _finalGradesService.GetAllAsync();

            // remove artificial, useless subjects
            var filteredSubjectApproaches =
                subjectApproaches.Where(subjectApproach =>
                    !subjectApproach.Acronym.All(c => c >= '0' && c <= '9')
                    && subjectApproach.Acronym.Length >= 3).ToList();

            string FindHighestSemesterLiteral(IEnumerable<SubjectApproach> approachesList)
            {
                var semesterLiteralAndShortConverter = new SemesterLiteralAndShortConverter();
                short highestSemester = default;
                foreach (var approach in approachesList)
                {
                    var literalAsShort =
                        semesterLiteralAndShortConverter.LiteralToShort(semesterLiteral: approach.SemesterLiteral);
                    if (literalAsShort > highestSemester)
                        highestSemester = literalAsShort;
                }

                return semesterLiteralAndShortConverter.ShortToString(semesterShort: highestSemester);
            }

            var inProgressApproaches =
                filteredSubjectApproaches.Where(predicate: x =>
                        x.SemesterLiteral == FindHighestSemesterLiteral(approachesList: filteredSubjectApproaches))
                    .ToList();

            var finishedApproaches = filteredSubjectApproaches.Where(predicate: x =>
                x.SemesterLiteral != FindHighestSemesterLiteral(approachesList: filteredSubjectApproaches)).ToList();
            finishedApproaches.SortStable(comparison: (x, y) => string.Compare(strA: x.Acronym, strB: y.Acronym,
                comparisonType: StringComparison.CurrentCultureIgnoreCase));

            foreach (var inProgressApproach in inProgressApproaches)
                SubjectApproaches.Add(item: inProgressApproach);


            foreach (var finishedApproach in finishedApproaches)
                SubjectApproaches.Add(item: finishedApproach);

        }

        public void GetPartialGradesContainer(string subjectId, string semesterLiteral)
        {
            DetailPaneInitilizationTaskNotifier = NotifyTask.Create(task:
                _partialGradesService.GetAsync(semesterLiteral: semesterLiteral, subjectId: subjectId));
        }
    }
}
