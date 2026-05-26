using LibSys.Services;
using LibSys.Models;

namespace LibSys
{
	public partial class SettingsForm : Form
	{
		private readonly IDataQueryService _queryService;
		private readonly IArchiveManager _archiveManager;

		public SettingsForm()
		{
			InitializeComponent();
			_queryService = new DataQueryService();
			_archiveManager = new ArchiveManager();
			this.Load += SettingsForm_Load;
		}

		private void SettingsForm_Load(object? sender, EventArgs e)
		{
			cmbDictType.Items.Clear();

			cmbDictType.Items.AddRange("Автори", "Видавництва", "Теги (Жанри)", "Книги");
			cmbDictType.SelectedIndex = 0;

			LoadSettings();
			RefreshDictionary();
		}

		public async void LoadSettings()
		{
			var settings = await _queryService.GetSystemSettingsAsync();
			if (settings == null) return;

			numMaxBooks.Value = settings.MaxBooksPerReader;
			numShortLoan.Value = settings.ShortLoanDays;
			numLongLoan.Value = settings.LongLoanDays;
			numBanDays.Value = settings.BanDaysForShortLoan;
			numPenaltyBase.Value = settings.PenaltyBaseAmount;
			numPenaltyPercent.Value = settings.PenaltyPercent * 100m;
			numNewBookAge.Value = settings.NewBookAgeThreshold;
			numRareCopies.Value = settings.RareBookCopiesThreshold;
		}

		private async void btnSaveSettings_Click(object sender, EventArgs e)
		{
			var settings = await _queryService.GetSystemSettingsAsync();
			if (settings == null) return;

			settings.MaxBooksPerReader = (int)numMaxBooks.Value;
			settings.ShortLoanDays = (int)numShortLoan.Value;
			settings.LongLoanDays = (int)numLongLoan.Value;
			settings.BanDaysForShortLoan = (int)numBanDays.Value;
			settings.PenaltyBaseAmount = numPenaltyBase.Value;
			settings.PenaltyPercent = numPenaltyPercent.Value / 100m;
			settings.NewBookAgeThreshold = (int)numNewBookAge.Value;
			settings.RareBookCopiesThreshold = (int)numRareCopies.Value;

			await _queryService.SaveSystemSettingsAsync(settings);

			MessageBox.Show("Налаштування успішно оновлено!", "Успіх", MessageBoxButtons.OK,
				MessageBoxIcon.Information);
		}

		private void btnCancelSettings_Click(object sender, EventArgs e) => LoadSettings();

		public async void RefreshDictionary()
		{
			bool showArchived = chkShowArchived.Checked;
			string type = cmbDictType.SelectedItem?.ToString() ?? "";
			string search = txtSearchDict.Text.Trim();

			if (type == "Автори")
			{
				var authors = await _queryService.GetAuthorsFilteredAsync(showArchived, search);
				lbDictionary.DataSource = authors.Select(a => new
					{ Id = a.Id, Display = $"{a.LastName} {a.FirstName} {a.Pseudonym}".Trim() }).ToList();
			}
			else if (type == "Видавництва")
			{
				var publishers = await _queryService.GetPublishersFilteredAsync(showArchived, search);
				lbDictionary.DataSource = publishers.Select(p => new { Id = p.Id, Display = p.Name }).ToList();
			}
			else if (type == "Теги (Жанри)")
			{
				var tags = await _queryService.GetTagsFilteredAsync(showArchived, search);
				lbDictionary.DataSource = tags.Select(t => new { Id = t.Id, Display = t.Name }).ToList();
			}
			else if (type == "Книги")
			{
				var books = await _queryService.GetBooksFilteredAsync(showArchived, search);
				lbDictionary.DataSource = books.Select(b => new { Id = b.Id, Display = b.Title }).ToList();
			}

			lbDictionary.DisplayMember = "Display";
			lbDictionary.ValueMember = "Id";

			btnToggleArchive.Text = showArchived ? "Відновити з архіву" : "Видалити / Архівувати";
			btnToggleArchive.BackColor = showArchived ? UIColors.Success : UIColors.Error;
		}

		private void cmbDictType_SelectedIndexChanged(object sender, EventArgs e) => RefreshDictionary();
		private void chkShowArchived_CheckedChanged(object sender, EventArgs e) => RefreshDictionary();
		private void txtSearchDict_TextChanged(object sender, EventArgs e) => RefreshDictionary();

		private async void btnToggleArchive_Click(object sender, EventArgs e)
		{
			if (lbDictionary.SelectedItem == null) return;

			var selectedItem = (dynamic)lbDictionary.SelectedItem;
			int id = selectedItem.Id;
			string type = cmbDictType.SelectedItem?.ToString() ?? "";
			bool isArchiving = !chkShowArchived.Checked;

			OperationResult result = null;
			if (isArchiving)
			{
				// Отримуємо попередження (перший виклик)
				switch (type)
				{
					case "Автори": result = await _archiveManager.ArchiveAuthorAsync(id, false); break;
					case "Видавництва": result = await _archiveManager.ArchivePublisherAsync(id, false); break;
					case "Теги (Жанри)": result = await _archiveManager.ArchiveTagAsync(id, false); break;
					case "Книги": result = await _archiveManager.ArchiveBookAsync(id, false); break;
				}

				if (result != null && !result.IsSuccess)
				{
					if (MessageBox.Show(result.Message + "\n\nПродовжити?", "Підтвердження",
						    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
					{
						// Повторний виклик з confirmed = true
						switch (type)
						{
							case "Автори": result = await _archiveManager.ArchiveAuthorAsync(id, true); break;
							case "Видавництва": result = await _archiveManager.ArchivePublisherAsync(id, true); break;
							case "Теги (Жанри)": result = await _archiveManager.ArchiveTagAsync(id, true); break;
							case "Книги": result = await _archiveManager.ArchiveBookAsync(id, true); break;
						}
					}
					else return;
				}
			}
			else
			{
				// Відновлення – без попереджень
				switch (type)
				{
					case "Автори": result = await _archiveManager.RestoreAuthorAsync(id); break;
					case "Видавництва": result = await _archiveManager.RestorePublisherAsync(id); break;
					case "Теги (Жанри)": result = await _archiveManager.RestoreTagAsync(id); break;
					case "Книги": result = await _archiveManager.RestoreBookAsync(id); break;
				}
			}

			if (result != null && result.IsSuccess)
			{
				var catalog = MainForm.Instance.GetCachedForm<CatalogForm>();
				await catalog.FullRefresh();
				RefreshDictionary();
				MessageBox.Show(result.Message, "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else if (result != null && !result.IsSuccess)
			{
				MessageBox.Show(result.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}