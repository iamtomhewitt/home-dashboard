const setBackgroundColour = (colour?: string) => {
  const body = document.getElementById('body');

  if (body && colour) {
    body.style.backgroundColor = colour;
  }
  else {
    console.warn('Could not find element by ID \'body\', or no colour parameter supplied');
  }
};

export const dashboard = {
  setBackgroundColour,
};