public interface EditableGameObject
{
	void notifyChanged();

	void deselect();

	void select();

	bool selectable();

	string getDescription();
}
