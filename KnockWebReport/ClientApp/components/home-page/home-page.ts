import * as ko from 'knockout';




class HomePageViewModel {
    public show = ko.observable(false);
    public url = ko.observable('');

    upload(file: Blob) {
        var files = new FormData();
        files.append("files", file)
        console.log(files);
        if (file != null) {
            fetch('api/SampleData/Upload', {
                method: 'POST',
                body: files
            })
                .then(response => response.text())
                .then(data => {
                    this.url("api/SampleData/ShowCube?name=" + data)
                });
            this.show(true);
        }
    }
}

export default { viewModel: HomePageViewModel, template: require('./home-page.html') };




